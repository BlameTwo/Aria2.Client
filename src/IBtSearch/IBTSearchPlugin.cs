using IBtSearch.Bases;
using IBtSearch.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IBtSearch;

public interface IBTSearchPlugin: IAria2Plugin
{
    public IAsyncEnumerable<BTSearchResult> SearchAsync(string query, CancellationToken token = default);

    public string Orgin { get; }
}

public class AsyncEnumerable<T> : IAsyncEnumerable<T>
{
    private readonly IAsyncEnumerator<T> _enumerator;

    public AsyncEnumerable(IAsyncEnumerator<T> enumerator)
    {
        _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
    }


    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _enumerator;
    }
}
