using FacadeMaster.Core.Models;
using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FacadeMaster.UI
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private readonly IProjectService _service;
        public EngProject Project { get; set; }
        public string Text { get; set; } = "Hello";

        public MainWindowModel()
        {
            _service = new ProjectService(new ProjectRepository());

            Project = _service.GetById(Guid.Parse("d9b1c8e7-5c3a-4f0b-9e5a-2f1e5c8a9b0d"));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
