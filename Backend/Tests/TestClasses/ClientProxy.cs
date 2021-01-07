using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.TestClasses
{
    public class ClientProxy : IClientProxy
    {
        public int SendCoreAsyncInvokedCount { get; set; } = 0;

        public virtual Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default)
        {
            SendCoreAsyncInvokedCount += 1;
            return Task.CompletedTask;
        }
    }
}
