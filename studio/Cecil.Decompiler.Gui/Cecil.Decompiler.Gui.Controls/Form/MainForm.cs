﻿using System.Windows.Forms;
using Cecil.Decompiler.Gui.Actions;
using Cecil.Decompiler.Gui.Services;
using Cecil.Decompiler.Gui.Tests;
using Cecil.Decompiler.Languages;
using Cecil.Decompiler.Gui.Collections;
using System.Reflection;
using System;

namespace Cecil.Decompiler.Gui.Controls
{
    internal partial class MainForm : Form, IActionManager, IBarManager 
    {
        ActionCollection actions = new ActionCollection();
        BarCollection bars = new BarCollection();

        #region Registration
        private void RegisterLanguages()
        {
            ILanguage selected;
            ILanguageCollection languages;

            languages = toolbarControl.LanguageManager.Languages;
            languages.Add(new CSharp());
            languages.Add(new CSharpV1());
            languages.Add(new CSharpV2());
            languages.Add(selected = new CSharpV3());

            languages.Add(new VisualBasic());
            languages.Add(new VisualBasic7());
            languages.Add(new VisualBasic8());
            languages.Add(new VisualBasic9());

            toolbarControl.LanguageManager.ActiveLanguage = selected;
        }

        private void RegisterServices()
        {
            ServiceProvider serviceProvider = ServiceProvider.GetInstance();
            serviceProvider.RegisterService(typeof(IServiceProvider), serviceProvider);
            serviceProvider.RegisterService(typeof(IAssemblyManager), assemblyManager);
            serviceProvider.RegisterService(typeof(IAssemblyBrowser), assemblyBrowserControl);
            serviceProvider.RegisterService(typeof(ILanguageManager), toolbarControl.LanguageManager);
            serviceProvider.RegisterService(typeof(IWindowManager), windowStackerControl);
        }

        private void RegisterBars()
        {
            toolbarControl.Name = BarNames.Toolbar.ToString();
            bars.Add(toolbarControl);

            menuControl.Name = BarNames.Menu.ToString();
            bars.Add(menuControl);

            statusbarControl.Name = BarNames.Status.ToString();
            bars.Add(statusbarControl);
        }

        private void RegisterActions()
        {
            actions.Add(new LoadAssemblyAction());
            actions.Add(new UnloadAssemblyAction());
            actions.Add(new DisassembleAction());
            actions.Add(new GoBackAction());
            actions.Add(new GoForwardAction());
            actions.Add(new AnalyzeAction());
            actions.Add(new VoidAction());
        }
        #endregion

        #region IActionManager
        IActionCollection IActionManager.Actions
        {
            get
            {
                return actions;
            }
        }

        #endregion

        #region IBarManager
        IBarCollection IBarManager.Bars
        {
            get
            {
                return bars;
            }
        }
        #endregion

        #region Methods
        public MainForm()
        {
            if (!DesignMode)
            {
                RegisterActions();
                ServiceProvider serviceProvider = ServiceProvider.GetInstance();
                serviceProvider.RegisterService(typeof(IActionManager), this);
                serviceProvider.RegisterService(typeof(IBarManager), this);
            }

            InitializeComponent();

            if (!DesignMode)
            {
                toolbarControl.WireItems();
                menuControl.WireItems();

                RegisterBars();
                RegisterLanguages();
                RegisterServices();

                breadCrumbControl.Wire();
            }

            // Testing
            // TestPlugin plugin = new TestPlugin();
            // plugin.Load(ServiceProvider.GetInstance());

            // Testing - dll plugins
            // Assembly reflexil = Assembly.Load("Reflexil");
            // Type type = reflexil.GetType("Reflexil.Utils.ReflexilPackage");
            // (Activator.CreateInstance(type) as IPlugin).Load(ServiceProvider.GetInstance());

        }
        #endregion

    }
}
