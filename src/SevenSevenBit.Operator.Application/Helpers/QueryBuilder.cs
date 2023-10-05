namespace SevenSevenBit.Operator.Application.Helpers;

using System.Text;
using NodaTime;
using SevenSevenBit.Operator.Domain.Enums;

public static class QueryBuilder
{
    public static string GetFilter(params (string Property, Type ValueType, object Value, FilterOptions? Option)[] filters)
    {
        var filterBuilder = new StringBuilder();

        foreach (var filter in filters)
        {
            if (filter.Value != null)
            {
                var convertedValue = Convert.ChangeType(filter.Value, filter.ValueType);
                filterBuilder.AppendFilterQuery(filter.Property, convertedValue, filter.Option);
            }
        }

        return filterBuilder.Length == 0 ? null : filterBuilder.ToString();
    }

    private static void AppendFilter(
        this StringBuilder filter,
        string columnName,
        Enum value,
        FilterOptions? comparison = FilterOptions.IsEqualTo)
    {
        filter.AppendAnd();

        switch (comparison)
        {
            case FilterOptions.IsEqualTo:
                filter.Append($"{columnName} = \"{value}\"");
                break;
            case FilterOptions.IsNotEqualTo:
                filter.Append($"{columnName} <> \"{value}\"");
                break;
            default:
                throw new ArgumentException($"FilterOption {comparison} is not valid for enum properties");
        }
    }

    private static void AppendFilter(
        this StringBuilder filter,
        string columnName,
        string value,
        FilterOptions? comparison = FilterOptions.IsEqualTo)
    {
        filter.AppendAnd();

        switch (comparison)
        {
            case FilterOptions.StartsWith:
                filter.Append($"{columnName}.StartsWith(\"{value}\")");
                break;
            case FilterOptions.EndsWith:
                filter.Append($"{columnName}.EndsWith(\"{value}\")");
                break;
            case FilterOptions.Contains:
                filter.Append($"{columnName}.Contains(\"{value}\")");
                break;
            case FilterOptions.DoesNotContain:
                filter.Append($"NOT {columnName}.Contains(\"{value}\")");
                break;
            case FilterOptions.IsEmpty:
                filter.Append($"{columnName} = \"\"");
                break;
            case FilterOptions.IsNotEmpty:
                filter.Append($"{columnName} <> \"\"");
                break;
            case FilterOptions.IsEqualTo:
                filter.Append($"{columnName} = \"{value}\"");
                break;
            case FilterOptions.IsNotEqualTo:
                filter.Append($"{columnName} <> \"{value}\"");
                break;
            default:
                throw new ArgumentException("Invalid filter comparison for string value.");
        }
    }

    private static void AppendFilter(
        this StringBuilder filter,
        string columnName,
        LocalDateTime value,
        FilterOptions? comparison = FilterOptions.IsEqualTo)
    {
        filter.AppendAnd();
        var dateValue = value.ToDateTimeUnspecified().ToString("O");

        switch (comparison)
        {
            case FilterOptions.IsEmpty:
                filter.Append($"{columnName} = \"\"");
                break;
            case FilterOptions.IsNotEmpty:
                filter.Append($"{columnName} <> \"\"");
                break;
            case FilterOptions.IsEqualTo:
                filter.Append($"{columnName} = \"{dateValue}\"");
                break;
            case FilterOptions.IsNotEqualTo:
                filter.Append($"{columnName} <> \"{dateValue}\"");
                break;
            case FilterOptions.IsGreaterThan:
                filter.Append($"{columnName} > \"{dateValue}\"");
                break;
            case FilterOptions.IsGreaterThanOrEqualTo:
                filter.Append($"{columnName} >= \"{dateValue}\"");
                break;
            case FilterOptions.IsLessThan:
                filter.Append($"{columnName} < \"{dateValue}\"");
                break;
            case FilterOptions.IsLessThanOrEqualTo:
                filter.Append($"{columnName} <= \"{dateValue}\"");
                break;
            default:
                throw new ArgumentException("Invalid filter comparison for string value.");
        }
    }

    private static void AppendFilter(
        this StringBuilder filter,
        string columnName,
        Guid value,
        FilterOptions? comparison = FilterOptions.IsEqualTo)
    {
        if (value == Guid.Empty)
        {
            return;
        }

        filter.AppendAnd();

        switch (comparison)
        {
            case FilterOptions.IsEqualTo:
                filter.Append($"{columnName} = \"{value}\"");
                break;
            case FilterOptions.IsNotEqualTo:
                filter.Append($"{columnName} <> \"{value}\"");
                break;
            default:
                throw new ArgumentException("Invalid filter comparison for Guid value.");
        }
    }

    private static void AppendFilter(
        this StringBuilder filter,
        string columnName,
        object value,
        FilterOptions? comparison = FilterOptions.IsEqualTo)
    {
        filter.AppendAnd();

        switch (comparison)
        {
            case FilterOptions.IsGreaterThan:
                filter.Append($"{columnName} > {value}");
                break;
            case FilterOptions.IsGreaterThanOrEqualTo:
                filter.Append($"{columnName} >= {value}");
                break;
            case FilterOptions.IsLessThan:
                filter.Append($"{columnName} < {value}");
                break;
            case FilterOptions.IsLessThanOrEqualTo:
                filter.Append($"{columnName} <= {value}");
                break;
            case FilterOptions.IsEqualTo:
                filter.Append($"{columnName} = {value}");
                break;
            case FilterOptions.IsNotEqualTo:
                filter.Append($"{columnName} <> {value}");
                break;
            default:
                throw new ArgumentException($"FilterOption {comparison} is not valid for numeric properties");
        }
    }

    private static void AppendFilterQuery<TValue>(
        this StringBuilder filter,
        string columnName,
        TValue value,
        FilterOptions? comparison = FilterOptions.IsEqualTo)
    {
        switch (value)
        {
            case string strValue:
                AppendFilter(filter, columnName, strValue, comparison);
                break;
            case LocalDateTime dateValue:
                AppendFilter(filter, columnName, dateValue, comparison);
                break;
            case Enum enumValue:
                filter.AppendFilter(columnName, enumValue, comparison);
                break;
            case Guid guidValue:
                filter.AppendFilter(columnName, guidValue, comparison);
                break;
            default:
                filter.AppendFilter(columnName, value, comparison);
                break;
        }
    }

    private static void AppendAnd(this StringBuilder filter)
    {
        if (filter.Length > 0)
        {
            filter.Append(" and ");
        }
    }
}