using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParlamentSimulation_ex2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    namespace ParlamentSimulation
    {
        public class Parlament
        {
            public readonly Parliamentarian[] ParlimentarianList;

            internal List<bool> VotingList;

            private readonly string VotingTopic;

            public event EventHandler StartVotingEvent;

            public event EventHandler EndVotingEvent;

            public Parlament(int x, string votingTopic)
            {
                VotingTopic = votingTopic;
                ParlimentarianList = new Parliamentarian[x];
                VotingList = new List<bool>();

                for (int i = 0; i < x; i++)
                {
                    ParlimentarianList[i] = new Parliamentarian(this);

                    StartVotingEvent += ParlimentarianList[i].SetVoteTrue;
                    EndVotingEvent += ParlimentarianList[i].SetVoteFalse;
                }
            }
            public void StartVoting(string topic)
            {
                if (VotingTopic == topic.Trim().ToLower())
                {
                    OnStartVotingEvent(EventArgs.Empty);
                    Console.WriteLine("Głosowanie rozpoczęło się.");
                }
            }

            public void EndVoting()
            {
                OnEndVotingEvent(EventArgs.Empty);
                Console.WriteLine("Głosowanie zakończyło się.");
                int YeaCounter = 0;
                int NayCounter = 0;
                foreach (bool vote in VotingList)
                {
                    if (vote)
                    {
                        YeaCounter++;
                    }
                    else
                    {
                        NayCounter++;
                    }
                }
                Console.WriteLine($"głosowanie nad {VotingTopic} Głosów za {YeaCounter} Głosów przeciw {NayCounter}");
                VotingList = new List<bool>();
            }
            protected virtual void OnStartVotingEvent(EventArgs e)
            {
                StartVotingEvent?.Invoke(this, e);
            }

            protected virtual void OnEndVotingEvent(EventArgs e)
            {
                EndVotingEvent?.Invoke(this, e);
            }

            internal void ReadVote(object sender, VoteEventArgs e)
            {

                VotingList.Add(e.Vote);
            }
        }

        public class Parliamentarian
        {
            private bool CanVote;

            public event EventHandler<VoteEventArgs>? Voted;

            public Parliamentarian(Parlament p)
            {
                CanVote = false;
                Voted += p.ReadVote;
            }

            internal void SetVoteTrue(object sender, EventArgs e)
            {
                CanVote = true;
            }

            internal void SetVoteFalse(object sender, EventArgs e)
            {
                CanVote = false;
            }
            private bool vote()
            {
                CanVote = false;
                Random rand = new();
                return rand.Next(0, 2) == 0;
            }

            public void Vote()
            {
                if (CanVote)
                {
                    OnVoted(new VoteEventArgs(vote()));
                }
                else
                {
                    Console.WriteLine("Głosowanie aktualnie się nie odbywa lub parlamentarzysta oddał już głos");
                }
            }

            protected virtual void OnVoted(VoteEventArgs e)
            {
                Voted?.Invoke(this, e);
            }
        }

        public class VoteEventArgs : EventArgs
        {
            public bool Vote;

            public VoteEventArgs(bool vote)
            {
                Vote = vote;
            }
        }
    }

}
