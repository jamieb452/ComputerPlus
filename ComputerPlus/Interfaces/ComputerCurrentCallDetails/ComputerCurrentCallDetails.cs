﻿using System;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using Rage;
using Rage.Forms;
using Gwen.Control;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;

namespace ComputerPlus
{
    internal class ComputerCurrentCallDetails : GwenForm
    {
        private Button btn_main;
        private ListBox list_calls;
        private Label lbl_c_unit, lbl_c_time, lbl_c_status, lbl_c_call;
        private Label lbl_a_id, lbl_a_time, lbl_a_call, lbl_a_loc, lbl_a_stat, lbl_a_unit, lbl_a_resp,
            lbl_a_desc, lbl_a_peds, lbl_a_vehs;
        private TextBox out_id, out_date, out_time, out_call, out_loc, out_stat, out_unit, out_resp;
        private MultilineTextBox out_desc, out_peds, out_vehs;
        private Base base_calls, base_active;
        private TabControl tc_main;
        internal static GameFiber form_main = new GameFiber(OpenMainMenuForm);

        public ComputerCurrentCallDetails() : base(typeof(ComputerCurrentCallDetailsTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.btn_main.Clicked += this.MainMenuButtonClickedHandler;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            CreateComponents();
            //FillCallDetails();
        }

        public void CreateComponents()
        {
            /***** Main Tab Control *****/
            tc_main = new TabControl(this);
            tc_main.SetPosition(15, 12);
            tc_main.SetSize(485, 292);

            /***** Call List Tab *****/
            // base container
            base_calls = new Base(this);
            base_calls.SetPosition(0, 0);
            base_calls.SetSize(485, 200);

            // calls listbox
            list_calls = new ListBox(base_calls);
            list_calls.SetPosition(0, 18);
            list_calls.SetSize(472, 240);

            // "Unit" label
            lbl_c_unit = new Label(base_calls);
            lbl_c_unit.Text = "Unit";
            lbl_c_unit.SetPosition(3, 1);
            lbl_c_unit.SetSize(26, 13);

            // "Time" label
            lbl_c_time = new Label(base_calls);
            lbl_c_time.Text = "Time";
            lbl_c_time.SetPosition(50, 1);
            lbl_c_time.SetSize(30, 13);

            // "Status" label
            lbl_c_status = new Label(base_calls);
            lbl_c_status.Text = "Status";
            lbl_c_status.SetPosition(96, 1);
            lbl_c_status.SetSize(37, 13);

            // "Call" label
            lbl_c_call = new Label(base_calls);
            lbl_c_call.Text = "Call";
            lbl_c_call.SetPosition(162, 1);
            lbl_c_call.SetSize(24, 13);

            /***** Active Call Tab *****/
            // base container
            base_active = new Base(this);
            base_active.SetPosition(0, 0);
            base_active.SetSize(485, 200);

            // "ID No." label
            lbl_a_id = new Label(base_active);
            lbl_a_id.Text = "ID No.";
            lbl_a_id.SetPosition(26, 6);
            lbl_a_id.SetSize(38, 13);
            // "ID No." textbox
            out_id = new TextBox(base_active);
            out_id.SetPosition(70, 3);
            out_id.SetSize(175, 20);
            out_id.KeyboardInputEnabled = false;

            // "Time" label
            lbl_a_time = new Label(base_active);
            lbl_a_time.Text = "Time";
            lbl_a_time.SetPosition(267, 7);
            lbl_a_time.SetSize(30, 13);
            // "Time" date textbox
            out_date = new TextBox(base_active);
            out_date.SetPosition(309, 3);
            out_date.SetSize(66, 20);
            out_date.KeyboardInputEnabled = false;
            // "Time" time textbox
            out_time = new TextBox(base_active);
            out_time.SetPosition(381, 3);
            out_time.SetSize(66, 20);
            out_time.KeyboardInputEnabled = false;

            // "Situation" label
            lbl_a_call = new Label(base_active);
            lbl_a_call.Text = "Situation";
            lbl_a_call.SetPosition(17, 33);
            lbl_a_call.SetSize(48, 13);
            // "Situation" textbox
            out_call = new TextBox(base_active);
            out_call.SetPosition(70, 29);
            out_call.SetSize(377, 20);
            out_call.KeyboardInputEnabled = false;

            // "Location" label
            lbl_a_loc = new Label(base_active);
            lbl_a_loc.Text = "Location";
            lbl_a_loc.SetPosition(18, 58);
            lbl_a_loc.SetSize(48, 13);
            // "Location" textbox
            out_loc = new TextBox(base_active);
            out_loc.SetPosition(70, 55);
            out_loc.SetSize(377, 20);
            out_loc.KeyboardInputEnabled = false;

            // "Status" label
            lbl_a_stat = new Label(base_active);
            lbl_a_stat.Text = "Status";
            lbl_a_stat.SetPosition(27, 84);
            lbl_a_stat.SetSize(37, 13);
            // "Status" textbox
            out_stat = new TextBox(base_active);
            out_stat.SetPosition(70, 81);
            out_stat.SetSize(106, 20);
            out_stat.KeyboardInputEnabled = false;

            // "Unit" label
            lbl_a_unit = new Label(base_active);
            lbl_a_unit.Text = "Unit";
            lbl_a_unit.SetPosition(183, 84);
            lbl_a_unit.SetSize(26, 13);
            // "Unit" textbox
            out_unit = new TextBox(base_active);
            out_unit.SetPosition(214, 81);
            out_unit.SetSize(86, 20);
            out_unit.KeyboardInputEnabled = false;

            // "Response" label
            lbl_a_resp = new Label(base_active);
            lbl_a_resp.Text = "Response";
            lbl_a_resp.SetPosition(309, 84);
            lbl_a_resp.SetSize(55, 13);
            // "Response" textbox
            out_resp = new TextBox(base_active);
            out_resp.SetPosition(368, 81);
            out_resp.SetSize(79, 20);
            out_resp.KeyboardInputEnabled = false;

            // "Comments" label
            lbl_a_desc = new Label(base_active);
            lbl_a_desc.Text = "Comments";
            lbl_a_desc.SetPosition(8, 113);
            lbl_a_desc.SetSize(56, 13);
            // "Comments" textbox
            out_desc = new MultilineTextBox(base_active);
            out_desc.SetPosition(70, 110);
            out_desc.SetSize(377, 45);
            out_desc.KeyboardInputEnabled = false;

            // "Persons" label
            lbl_a_peds = new Label(base_active);
            lbl_a_peds.Text = "Persons";
            lbl_a_peds.SetPosition(19, 161);
            lbl_a_peds.SetSize(45, 13);
            // "Persons" textbox
            out_peds = new MultilineTextBox(base_active);
            out_peds.SetPosition(70, 161);
            out_peds.SetSize(377, 45);
            out_peds.KeyboardInputEnabled = false;

            // "Vehicles" label
            lbl_a_vehs = new Label(base_active);
            lbl_a_vehs.Text = "Vehicles";
            lbl_a_vehs.SetPosition(19, 215);
            lbl_a_vehs.SetSize(47, 13);
            // "Vehicles" textbox
            out_vehs = new MultilineTextBox(base_active);
            out_vehs.SetPosition(70, 212);
            out_vehs.SetSize(377, 45);
            out_vehs.KeyboardInputEnabled = false;

            // Add tabs and their corresponding containers
            tc_main.AddPage("Call List", base_calls);
            tc_main.AddPage("Active Call", base_active);

            list_calls.AddRow(String.Format("{0} {1, 10} {2, 15} {3, 25}", "1-A-12", "15:54", "At Scene", "OFC DROPPED DONUT"));
        }

        private void MainMenuButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            form_main = new GameFiber(OpenMainMenuForm);
            form_main.Start();
        }

        private static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }

        private void FillCallDetails()
        {
            String callText = "";

            if (EntryPoint.gActiveCallout != null)
            {
                /*callText += EntryPoint.gActiveCallout.FullName;
                callText += Environment.NewLine;
                callText += Environment.NewLine;
                callText += EntryPoint.gActiveCallout.Description;*/
            }

            //output_info.Text = callText;
        }
    }
}