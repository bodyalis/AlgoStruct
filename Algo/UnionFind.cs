namespace HuffmanAlgorithm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public sealed class UnionFind
{
    private readonly int[] parent;
    private readonly int[] rank;
    
    public UnionFind(int size)
    {
        parent = new int[size];
        rank = new int[size];
        for (int i = 0; i < size; i++)
        {
            parent[i] = i;
            rank[i] = 0;  // Начальный rank = 0
        }
    }
    
    public int Find(int x)
    {
        while (parent[x] != x)
            x = parent[x];
        return x;
    }
    
    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);
        
        if (rootX == rootY) return;
        
        // Union by Rank
        if (rank[rootX] < rank[rootY])
            parent[rootX] = rootY;
        else if (rank[rootX] > rank[rootY])
            parent[rootY] = rootX;
        else
        {
            parent[rootY] = rootX;
            rank[rootX]++;  // rank++ ТОЛЬКО при равенстве
        }
    }
}
