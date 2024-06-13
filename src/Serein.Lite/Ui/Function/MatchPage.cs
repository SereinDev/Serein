﻿using System;
using System.Windows.Forms;

using Serein.Core.Services.Data;

namespace Serein.Lite.Ui.Function;

public partial class MatchPage : UserControl
{
    private readonly MatchesProvider _matchesProvider;

    public MatchPage(MatchesProvider matchesProvider)
    {
        InitializeComponent();
        _matchesProvider = matchesProvider;
    }

    private void MatchListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        StatusStrip.Text = MatchListView.SelectedItems.Count > 0
            ? $"共{MatchListView.Items.Count}项；已选择{MatchListView.SelectedItems.Count}项"
            : $"共{MatchListView.Items.Count}项";
    }
}
