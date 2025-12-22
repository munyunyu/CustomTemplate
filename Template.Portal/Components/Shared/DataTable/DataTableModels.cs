using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Template.Portal.Components.Shared.DataTable
{
    public enum FilterCaseSensitivity
    {
        CaseSensitive = 0,
        CaseInsensitive = 1
    }

    public enum LogicalFilterOperator
    {
        And = 0,
        Or = 1
    }

    public enum DataTableSelectionMode
    {
        None = 0,
        Single = 1
    }

    internal sealed class DataTableColumnDefinition<TItem>
    {
        public string? Property { get; set; }
        public string? Title { get; set; }
        public string? Width { get; set; }

        public bool Sortable { get; set; }
        public bool Filterable { get; set; }

        public RenderFragment<TItem>? Template { get; set; }
    }

    public sealed class RowRenderEventArgs<TItem>
    {
        public RowRenderEventArgs(TItem data)
        {
            Data = data;
        }

        public TItem Data { get; }

        public IDictionary<string, object> Attributes { get; } = new Dictionary<string, object>();
    }

    internal sealed class DataTableComparers
    {
        internal static IComparer<object?> ObjectComparer { get; } = new ObjectToStringComparer();

        private sealed class ObjectToStringComparer : IComparer<object?>
        {
            public int Compare(object? x, object? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (x is null)
                {
                    return -1;
                }

                if (y is null)
                {
                    return 1;
                }

                if (x is IComparable comparable)
                {
                    try
                    {
                        return comparable.CompareTo(y);
                    }
                    catch
                    {
                    }
                }

                return StringComparer.OrdinalIgnoreCase.Compare(x.ToString(), y.ToString());
            }
        }
    }
}
