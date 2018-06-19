using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            switch (message.Text)
            {
                case "reset":
                    PromptDialog.Confirm(
                                        context,
                                        AfterResetAsync,
                                        "Are you sure you want to reset the count?",
                                        "Didn't get that!",
                                        promptStyle: PromptStyle.Auto);
                    break;
                default:

                    await context.PostAsync($"{this.count++}: You said {message.Text}");
                    context.Wait(MessageReceivedAsync);
                    break;
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count to 1.");
            }
            else
            {
                await context.PostAsync($"Did not reset count. Current count is {this.count}.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}