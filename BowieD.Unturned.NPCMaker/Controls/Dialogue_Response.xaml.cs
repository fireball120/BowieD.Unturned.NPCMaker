﻿using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using DiscordRPC;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Response.xaml
    /// </summary>
    public partial class Dialogue_Response : UserControl, IHasOrderButtons
    {
        public Dialogue_Response(NPC.NPCResponse startResponse = null)
        {
            InitializeComponent();
            Response = startResponse ?? new NPC.NPCResponse();
            mainText.Text = Response.mainText;
            txtBoxDialogueID.Value = Response.openDialogueId;
            txtBoxQuestID.Value = Response.openQuestId;
            txtBoxVendorID.Value = Response.openVendorId;
        }

        public NPC.NPCResponse Response { get; private set; }

        public bool IsCollapsed { get; private set; }

        public void Expand()
        {
            expandedGrid.Visibility = Visibility.Visible;
            collapsedGrid.Visibility = Visibility.Collapsed;
        }
        public void Collapse()
        {
            expandedGrid.Visibility = Visibility.Collapsed;
            collapsedGrid.Visibility = Visibility.Visible;
            collapsedText.Text = TextUtil.Shortify(mainText.Text, 24);
        }

        public void RebuildResponse()
        {
            Response.mainText = mainText.Text;
            Response.openDialogueId = (ushort)txtBoxDialogueID.Value;
            Response.openQuestId = (ushort)txtBoxQuestID.Value;
            Response.openVendorId = (ushort)txtBoxVendorID.Value;
        }

        #region EVENTS
        private void EditRewardsButton_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Response.rewards.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Reward, true)).ToList(), Universal_ItemList.ReturnType.Reward);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.txtEditorName.Text ?? "without name"}",
                State = "Creating reward for a dialogue response"
            };
            (MainWindow.DiscordManager as DiscordRPC.DiscordManager)?.SendPresence(presence);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Response.rewards = ulv.Values.Cast<Reward>().ToArray();
            MainWindow.Instance.MainWindowViewModel.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Response.conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, true)).ToList(), Universal_ItemList.ReturnType.Condition);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.txtEditorName.Text ?? "without name"}",
                State = "Creating condition for a dialogue response"
            };
            (MainWindow.DiscordManager as DiscordRPC.DiscordManager)?.SendPresence(presence);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Response.conditions = ulv.Values.Cast<Condition>().ToArray();
            MainWindow.Instance.MainWindowViewModel.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void MainText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Response.mainText = mainText.Text;
        }

        private void QuestSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.data.quests.Count() == 0)
            {
                return;
            }

            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Quest);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxQuestID.Value = (select.SelectedValue as NPC.NPCQuest).id;
            }
        }

        private void VendorSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.data.vendors.Count() == 0)
            {
                return;
            }

            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Vendor);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxVendorID.Value = (select.SelectedValue as NPC.NPCVendor).id;
            }
        }

        private void DialogueSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.data.dialogues.Count() == 0)
            {
                return;
            }

            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Dialogue);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxDialogueID.Value = (select.SelectedValue as NPC.NPCDialogue).id;
            }
        }

        private void ChangeVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.messages.Count > 1)
            {
                Message_TreeView mtv = new Message_TreeView(Response.visibleIn, MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.messages.Count);
                mtv.Owner = MainWindow.Instance;
                mtv.ShowDialog();
                if (mtv.DialogResult == true)
                {
                    int[] arr = mtv.AsIntArray;
                    Response.visibleIn = arr.Count() == 0 ? null : arr;
                }
            }
            else
            {
                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Dialogue_Reply_Visiblity_TwoOrLessMessages"]);
            }
        }

        private void TxtBoxVendorID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                Response.openVendorId = (ushort)e.NewValue.Value;
            }
        }

        private void TxtBoxQuestID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                Response.openQuestId = (ushort)e.NewValue.Value;
            }
        }
        private void TxtBoxDialogueID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                Response.openDialogueId = (ushort)e.NewValue.Value;
            }
        }
        #endregion

        public UIElement UpButton => orderButtonUp;
        public UIElement DownButton => orderButtonDown;
        public Transform Transform => animateTransform;

        private void Collapse_Button_Click(object sender, RoutedEventArgs e)
        {
            Collapse();
        }
        private void Expand_Button_Click(object sender, RoutedEventArgs e)
        {
            Expand();
        }
    }
}
