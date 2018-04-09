using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MetroFramework.Controls;
using MetroFramework.Forms;


namespace _8_Puzzle
{
    public partial class Form1 : MetroForm
    {
        private int[] _field = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        private  Dictionary<int, MetroPanel> _tiles;
        private List<KeyValuePair<int, MetroPanel>> _prevPosTiles;
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        void InitializeGame()
        {
            _tiles = new Dictionary<int, MetroPanel>()
            {
                {1, tile_1}, {2, tile_2}, {3, tile_3},
                {4, tile_4}, {5, tile_5}, {6, tile_6},
                {7, tile_7}, {8, tile_8}, {0, tile_9}
            };
            _prevPosTiles = _tiles.ToList();
            foreach (var t in _tiles)
            {
                if(t.Key==0)continue;
                t.Value.Click += MoveTile;
            }
            labelRes.Text = @"Успех!";
        }
        void MoveTile(object sender, EventArgs e)
        {
            var tileId = Array.IndexOf(_prevPosTiles.Select(x => x.Key).ToArray(), ((MetroPanel)sender).TabIndex);
            var res = Puzzle.move(_field, tileId).ToList();
            UpdateField(res);
        }

        private void UpdateField(List<int> res)
        {
            if (!res.SequenceEqual(_field))
            {
                var oldState = _prevPosTiles.Select(x => x.Value.Location).ToList();
                var newState = _tiles.OrderBy(t => res.IndexOf(t.Key)).ToList();
                for (var i = 0; i < newState.Count; i++)
                {
                    var temp = oldState[i];
                    newState[i].Value.Location = temp;
                }
                _prevPosTiles = newState;
                _field = res.ToArray();
            }
            labelRes.Text = res.SequenceEqual(Puzzle.goalState) ? @"Успех!" : @"Играет...";
        }
        private void ShowSolve(IEnumerable<int[]> res)
        {
            var resList = res.Select(x => x.ToList()).ToList();
            foreach (var nextState in resList)
            {
                if (!nextState.SequenceEqual(_field))
                {
                    var oldState = _prevPosTiles.Select(x => x.Value.Location).ToList();
                    var newState = _tiles.OrderBy(t => nextState.IndexOf(t.Key)).ToList();
                    for (var i = 0; i < newState.Count; i++)
                    {
                        var temp = oldState[i];
                        var index = i;
                        newState[i].Value.BeginInvoke((Action)(()=> { newState[index].Value.Location = temp; }));
                    }
                    _prevPosTiles = newState;
                    _field = nextState.ToArray();
                }
                labelRes.BeginInvoke(
                    (Action)
                    (() => { labelRes.Text = nextState.SequenceEqual(Puzzle.goalState) ? @"Успех!" : @"Играет..."; }));
                Thread.Sleep(1000);
            }
          
        }

        private void btnRnd_Click(object sender, EventArgs e)
        {
            var res = Puzzle.shuffle(_field, 20).ToList();
            UpdateField(res);
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            var res = Puzzle.solve(_field, Puzzle.goalState);
            if (res.Length == 1) return;
            var updateThread = new Thread(() => ShowSolve(res));
            updateThread.Start();
        }
    }
}
