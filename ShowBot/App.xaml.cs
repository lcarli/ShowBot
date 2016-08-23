using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Microsoft.Bot.Connector.DirectLine.Models;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ShowBot
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>

        public static Conversation ConversationInfo;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public static async Task<Message> messageToBot(string name, string message)
        {
            Message msg = new Message();
            msg.Text = message;
            msg.Id = name;

            var baseURI = new Uri("https://directline.botframework.com/api/conversations/");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", "3bYapmOCC04.cwA.60E.PCm8N5Cw3m0mBcZ6dNWdQuJFd68zh4hmheCJPm7OFWo");
            HttpResponseMessage response = await client.PostAsync(baseURI, new StringContent(""));
            ConversationInfo = JsonConvert.DeserializeObject<Conversation>(response.Content.ReadAsStringAsync().Result);
            if (response.IsSuccessStatusCode)
            {
                var conversationURI = baseURI + ConversationInfo.ConversationId + "/messages/";
                var jsonmsg = JsonConvert.SerializeObject(msg);
                response = await client.PostAsync(conversationURI, new StringContent(jsonmsg, Encoding.UTF8, "application/json"));
                string wtf = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    response = await client.GetAsync(conversationURI);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageSet BotMessage = JsonConvert.DeserializeObject<MessageSet>(response.Content.ReadAsStringAsync().Result);
                        return BotMessage.Messages[1];
                    }
                    else
                    {
                        msg.Id = "Error";
                        return msg;
                    }
                }
                else
                {
                    msg.Id = "Error";
                    return msg;
                }
            }
            else
            {
                msg.Id = "Error";
                return msg;
            }

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
