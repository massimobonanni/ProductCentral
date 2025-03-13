using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCentral.LogisticCLI.Commands
{
    internal abstract class CommandBase : Command
    {

        protected readonly ServiceProvider _serviceProvider;
        protected readonly CredentialManager _credentialManager;

        public CommandBase(string name, string description, ServiceProvider serviceProvider) : base(name, description)
        {
            _serviceProvider = serviceProvider;
            _credentialManager = _serviceProvider.GetRequiredService<CredentialManager>();
        }
    }

}
