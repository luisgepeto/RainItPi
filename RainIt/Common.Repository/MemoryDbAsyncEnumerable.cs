using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Repository
{
    internal class MemoryDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>
    {
        public MemoryDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public MemoryDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new MemoryDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        public IQueryProvider Provider
        {
            get { return new MemoryDbAsyncQueryProvider<T>(this); }
        }
    } 
}
