using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.SimulatorCore.Transport
{
    /// <summary>
    /// Wraps the byte array returned from the cloud so that it can be deserialized
    /// </summary>
    public class DeserializableCommand
    {
        private readonly CommandHistory _commandHistory;
        private readonly string _lockToken;

        public string CommandName => _commandHistory.Name;

        public DeserializableCommand(CommandHistory history, string lockToken)
        {
            this._commandHistory = history;
            this._lockToken = lockToken;
        }

        public DeserializableCommand(Microsoft.Azure.Devices.Client.Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Debug.Assert(
                !string.IsNullOrEmpty(message.LockToken),
                "message.LockToken is a null reference or empty string.");
            _lockToken = message.LockToken;

            byte[] messageBytes = message.GetBytes(); // this needs to be saved if needed later, because it can only be read once from the original Message

            string jsonData = Encoding.UTF8.GetString(messageBytes);
            _commandHistory = JsonConvert.DeserializeObject<CommandHistory>(jsonData);
        }

        public CommandHistory CommandHistory
        {
            get { return _commandHistory; }
        }

        public string LockToken
        {
            get
            {
                return _lockToken;
            }
        }
    }
}
