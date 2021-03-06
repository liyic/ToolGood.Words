﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Words.internals
{
    public abstract class BaseSearch
    {
        protected TrieNode _root = new TrieNode();
        protected TrieNode[] _first = new TrieNode[char.MaxValue + 1];


        public virtual void SetKeywords(ICollection<string> _keywords)
        {
            var first = new TrieNode[char.MaxValue + 1];
            var root = new TrieNode();
            foreach (var p in _keywords) {
                if (string.IsNullOrEmpty(p)) continue;

                var nd = first[p[0]];
                if (nd == null) {
                    nd = root.Add(p[0]);
                    first[p[0]] = nd;
                }
                for (int i = 1; i < p.Length; i++) {
                    nd = nd.Add(p[i]);
                }
                nd.SetResults(p);
            }
            this._first = first;

            Dictionary<TrieNode, TrieNode> links = new Dictionary<TrieNode, TrieNode>();
            foreach (var item in root.m_values) {
                TryLinks(item.Value, null, links);
            }

            foreach (var item in links) {
                item.Key.Merge(item.Value);
            }

            _root = root;
        }

        private void TryLinks(TrieNode node, TrieNode node2, Dictionary<TrieNode, TrieNode> links)
        {
            foreach (var item in node.m_values) {
                if (node2 == null) {
                    var nd = _first[item.Key];
                    if (nd == null) continue;
                    links[item.Value] = nd;
                    TryLinks(item.Value, nd, links);
                } else {

                    TrieNode tn;
                    if (node2.TryGetValue(item.Key, out tn)) {
                        links[item.Value] = tn;
                        TryLinks(item.Value, tn, links);
                    }
                }
            }
        }

    }
}
