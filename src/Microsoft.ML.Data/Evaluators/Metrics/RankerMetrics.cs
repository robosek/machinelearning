// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.ML.Runtime;
using Microsoft.ML.Runtime.Data;

namespace Microsoft.ML.Data
{
    public sealed class RankerMetrics
    {
        /// <summary>
        /// Normalized Discounted Cumulative Gain
        /// <a href="https://github.com/dotnet/machinelearning/tree/master/docs/images/ndcg.png"></a>
        /// </summary>
        public double[] Ndcg { get; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/Discounted_cumulative_gain">Discounted Cumulative gain</a>
        /// is the sum of the gains, for all the instances i, normalized by the natural logarithm of the instance + 1.
        /// Note that unline the Wikipedia article, ML.Net uses the natural logarithm.
        /// <a href="https://github.com/dotnet/machinelearning/tree/master/docs/images/dcg.png"></a>
        /// </summary>
        public double[] Dcg { get; }

        private static T Fetch<T>(IExceptionContext ectx, IRow row, string name)
        {
            if (!row.Schema.TryGetColumnIndex(name, out int col))
                throw ectx.Except($"Could not find column '{name}'");
            T val = default;
            row.GetGetter<T>(col)(ref val);
            return val;
        }

        internal RankerMetrics(IExceptionContext ectx, IRow overallResult)
        {
            VBuffer<double> Fetch(string name) => Fetch<VBuffer<double>>(ectx, overallResult, name);

            Dcg = Fetch(RankerEvaluator.Dcg).GetValues().ToArray();
            Ndcg = Fetch(RankerEvaluator.Ndcg).GetValues().ToArray();
        }
    }
}