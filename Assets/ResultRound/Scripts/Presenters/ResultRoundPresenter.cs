using System;
using TopicTwister.ResultRound.Shared.Interfaces;


namespace TopicTwister.ResultRound.Presenters
{
    public class ResultRoundPresenter
    {
        private readonly IResultRoundView _resultRoundView;

        public ResultRoundPresenter(IResultRoundView resultRoundView)
        {
            _resultRoundView = resultRoundView;
            _resultRoundView.OnLoad += OnLoadHandler;
        }

        ~ResultRoundPresenter()
        {
            this._resultRoundView.OnLoad -= OnLoadHandler;
        }

        private void OnLoadHandler()
        {
            Console.WriteLine("OnLoad event received.");
        }
    }
}
