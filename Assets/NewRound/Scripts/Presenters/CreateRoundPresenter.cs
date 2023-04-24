using System;
using TopicTwister.NewRound.Shared.Interfaces;


namespace TopicTwister.NewRound.Presenters
{
    public class CreateRoundPresenter : ICreateRoundPresenter
    {
        ICreateRoundView _view;

        public CreateRoundPresenter(ICreateRoundView view)
        {
            _view = view;
            _view.OnLoad += CreateRound;
        }

        public void CreateRound(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
