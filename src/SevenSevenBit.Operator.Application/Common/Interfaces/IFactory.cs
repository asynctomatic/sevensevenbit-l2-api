namespace SevenSevenBit.Operator.Application.Common.Interfaces;

public interface IFactory<in TIn, out TOut>
{
    TOut Create(TIn input);
}