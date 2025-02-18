using StudentsHelper.ViewModels.Abstract;
using System.Windows.Input;

namespace StudentsHelper.ViewModels
{
    public class AboutAppViewModel : BaseViewModel
    {
        #region constructor
        public AboutAppViewModel()
        {
            SendFeedbackCommand = new Command(
                async () =>
                {
                    string subject = "Zpětná vazba k aplikaci Pomocník studenta";
                    string body = "Ahoj,\n\nposílám ti zpětnou vazbu ke tvé aplikaci. Chtěl/a bych ti aplikaci pochválit / zkritizovat / nahlásit chybu:\n\n";
                    var msg = new EmailMessage
                    {
                        Subject = subject,
                        Body = body,
                        BodyFormat = EmailBodyFormat.PlainText,
                        To = ["daniel.rybar1@tul.cz"]
                    };
                    await Email.Default.ComposeAsync(msg);
                }
            );
        }
        #endregion

        #region commands
        public ICommand SendFeedbackCommand { get; private set; }
        #endregion
    }
}