using System;
using System.ComponentModel;

namespace QuUtils.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class SetModifiedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Modifier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        public SetModifiedEventArgs(object sender, bool value)
        {
            Modifier = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VmBase : INotifyPropertyChanged
    {
        private readonly CommandLibrary _commands;

        #region public constructor

        /// <summary>
        /// 
        /// </summary>
        public VmBase()
        {
            _commands = new CommandLibrary();
        }

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetModified(bool value)
        {
            var handler = PropertyModified;
            if (handler != null)
                handler(this, new SetModifiedEventArgs(this, value));
        }

        #endregion

        #region public properties

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CommandLibrary Commands
        {
            get { return _commands; }
        }

        #endregion

        #region event properties

        /// <summary>
        /// 
        /// </summary>
        public delegate void PropertyModifiedHandler(object sender, SetModifiedEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        [Description("This event is raised if a property has been modified.")]
        public event PropertyModifiedHandler PropertyModified;

        #endregion

        #region INotifyPropertyChanged members

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}