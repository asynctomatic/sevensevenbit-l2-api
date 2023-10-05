namespace SevenSevenBit.Operator.Infrastructure.NoSQL.Services;

using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Infrastructure.NoSQL.Options;
using StackExchange.Redis;

public class NoSqlService : INoSqlService
{
    private const string TxKeyPrefix = "TransactionIdService:Transactions";
    private const string TxCounterKeyPrefix = "TransactionIdService:StarkExInstances:TxCounter";
    private const string TxUnlockedCounterKeyPrefix = "TransactionIdService:StarkExInstances:UnlockedTxCounter";
    private readonly RedisOptions redisOptions;
    private readonly IConnectionMultiplexer conn;

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSqlService"/> class.
    /// </summary>
    /// <param name="conn">A connection to a Redis instance.</param>
    /// /// <param name="redisOptions">Configuration options for connecting to a Redis instance.</param>
    public NoSqlService(IConnectionMultiplexer conn, IOptions<RedisOptions> redisOptions)
    {
        this.conn = conn;
        this.redisOptions = redisOptions.Value;
    }

    /// <inheritdoc />
    public async Task UnlockTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId)
    {
        // This Lua script atomically executes the following operations in Redis:
        // Checks if a transaction id has been allocated for the given transaction key.
        // If it has, it gets the value for the key and adds it to the set of unlocked transaction ids.
        const string unlockScript = @"
        if redis.call(""EXISTS"", @transactionKey) == 1 then
          local starkexTransactionId = redis.call(""GET"", @transactionKey)
          redis.call(""ZADD"", @unlockedKey, starkexTransactionId, starkexTransactionId)
        end
        ";

        // Configure the parameters for the script.
        var parameters = new
        {
            transactionKey = FormatKey(TxKeyPrefix, transactionKey.ToString()),
            unlockedKey = FormatKey(TxUnlockedCounterKeyPrefix, starkExInstanceId.ToString()),
        };

        var db = conn.GetDatabase(redisOptions.DatabaseId);

        // TODO Test redis transactions
        var prepared = LuaScript.Prepare(unlockScript);

        await db.ScriptEvaluateAsync(prepared, parameters);
    }

    /// <inheritdoc />
    public async Task<long> LockTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId)
    {
        // This Lua script atomically executes the following operations in Redis:
        // Checks if a transaction id has already been allocated for the given transaction key.
        // If it has not, then it checks if there are any unlocked transaction ids available.
        // If there are, it pops one off the set and sets it as the value for the transaction key.
        // If there are no unlocked transaction ids, it increments the StarkEx instance transaction id counter and sets
        // that as the value for the transaction key.
        // Finally, it returns the value for the transaction key.
        const string lockScript = @"
        local ttl = tonumber(@ttl)

        if redis.call(""EXISTS"", @transactionKey) == 0 then
            local starkexTransactionId
            if (redis.call(""ZCARD"", @unlockedKey) == 0) then
                starkexTransactionId = redis.call(""INCR"", @starkExInstanceKey)
            else
                local result = redis.call(""ZPOPMIN"", @unlockedKey)
                starkexTransactionId = result[1]
            end 

            redis.call(""SET"", @transactionKey, starkexTransactionId, ""EX"", ttl)
        end

        return redis.call(""GET"", @transactionKey)";

        // Configure the parameters for the script.
        var parameters = new
        {
            transactionKey = FormatKey(TxKeyPrefix, transactionKey.ToString()),
            starkExInstanceKey = FormatKey(TxCounterKeyPrefix, starkExInstanceId.ToString()),
            unlockedKey = FormatKey(TxUnlockedCounterKeyPrefix, starkExInstanceId.ToString()),
            ttl = redisOptions.TtlInSeconds,
        };

        var db = conn.GetDatabase(redisOptions.DatabaseId);

        // TODO Test redis transactions
        var prepared = LuaScript.Prepare(lockScript);

        var result = await db.ScriptEvaluateAsync(prepared, parameters);

        return (long)result;
    }

    private static RedisKey FormatKey(string prefix, string suffix)
    {
        return (RedisKey)$"{prefix}:{suffix}";
    }
}