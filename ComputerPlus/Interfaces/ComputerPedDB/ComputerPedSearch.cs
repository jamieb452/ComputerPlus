﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Controllers.Models;

namespace ComputerPlus.Interfaces.ComputerPedDB
{
    sealed class ComputerPedSearch : GwenForm
    {
        ListBox list_collected_ids;
        ListBox list_manual_results;

        TextBox text_manual_name;

        internal ComputerPedSearch() : base(typeof(ComputerPedSearchTemplate))
        {
        }

        ~ComputerPedSearch()
        {
            list_manual_results.RowSelected -= onListItemSelected;
            list_collected_ids.RowSelected -= onListItemSelected;
            text_manual_name.SubmitPressed -= onSearchSubmit;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            text_manual_name.SetToolTipText("Name");
            this.Position = this.GetLaunchPosition();
            this.Window.DisableResizing();
            PopulateManualSearchPedsList();
            PopulateStoppedPedsList();
            list_manual_results.AllowMultiSelect = false;
            list_collected_ids.AllowMultiSelect = false;
            list_manual_results.RowSelected += onListItemSelected;
            list_collected_ids.RowSelected += onListItemSelected;
            text_manual_name.SubmitPressed += onSearchSubmit;
        }

        private void onSearchSubmit(Base sender, EventArgs arguments)
        {
            String name = text_manual_name.Text.ToLower();
            if (String.IsNullOrWhiteSpace(name)) return;
            ComputerPedController controller = ComputerPedController.Instance;
            var ped = controller.LookupPersona(name);
            if (ped != null)
            {
                if (!ped.Ped)
                {
                    text_manual_name.Error("The ped no longer exists");
                }
                else {
                    text_manual_name.ClearError();
                    list_manual_results.AddPed(ped);
                    ComputerPedController.LastSelected = ped;
                    ComputerPedController.ActivatePedView();
                }
            } else
            {
                text_manual_name.Error("No matches");
            }
        }


        public void PopulateManualSearchPedsList()
        {
            ComputerPedController controller = ComputerPedController.Instance;
            list_collected_ids.Clear();
            var results = controller.GetRecentSearches()
            .Where(x => x.Validate()).ToList(); 
            //@TODO choose if we want to remove null items from the list -- may cause user confusion
            if (results != null && results.Count > 0)
            {
                results.ForEach(x => list_manual_results.AddPed(x));
            }
        }

        public void PopulateStoppedPedsList()
        {
            ComputerPedController controller = ComputerPedController.Instance;
            var peds = controller.PedsCurrentlyStoppedByPlayer;
            list_collected_ids.Clear();
            foreach(var persona in peds.Select(x => controller.LookupPersona(x)))
            {
                list_collected_ids.AddPed(persona);
            }           
        }

        private void ClearSelections()
        {
            list_collected_ids.UnselectAll();
            list_manual_results.UnselectAll();
        }

        private void onListItemSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (arguments.SelectedItem.UserData is ComputerPlusEntity)
            {
                ComputerPedController.LastSelected = arguments.SelectedItem.UserData as ComputerPlusEntity;                
                ClearSelections();
                ComputerPedController.ActivatePedView();
            }
            else
            {
                Function.Log("ComputerPedSearch.onListItemSelected arguments were not valid");
            }         
        }      
    }
}
