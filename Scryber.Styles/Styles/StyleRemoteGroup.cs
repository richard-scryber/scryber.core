using System;
namespace Scryber.Styles
{
    public class StyleRemoteGroup : StyleGroup, IComponent, ILoadableComponent
    {

        public event InitializedEventHandler Initialized;
        public event LoadedEventHandler Loaded;

        protected virtual void OnInitialized(InitContext context)
        {
            if(null != this.Initialized)
            {
                this.Initialized(this, new InitEventArgs(context));
            }
        }

        protected virtual void OnLoaded(LoadContext load)
        {
            if(null != this.Loaded)
            {
                this.Loaded(this, new LoadEventArgs(load));
            }
        }

        public StyleRemoteGroup()
        {
        }

        public string ElementName { get; set; }

        public IDocument Document { get { return (null == this.Parent) ? null : this.Parent.Document;  } }

        public IComponent Parent { get; set; }

        /// <summary>
        /// Override the base implementation as we are a fully qualified component, and can have inner items.
        /// </summary>
        public override IComponent Owner
        {
            get { return this.Parent; }
            set { this.Parent = value; }
        }

        /// <summary>
        /// Gets or sets the source this group was loaded from
        /// </summary>
        public string LoadedSource { get; set; }

        /// <summary>
        /// Gets or sets the type of loading this group was from.
        /// </summary>
        public ParserLoadType LoadType { get; set; }


        //
        // methods
        //

        public void Init(InitContext context)
        {
            this.InnerItems.Init(context);
            this.OnInitialized(context);
        }

        public void Load(LoadContext context)
        {
            this.InnerItems.Load(context);
            this.OnLoaded(context);
        }

        public override string MapPath(string path)
        {
            bool isfile;
            var service = ServiceProvider.GetService<IPathMappingService>();

            if (!string.IsNullOrEmpty(this.LoadedSource))
            {
                return service.MapPath(this.LoadType, path, this.LoadedSource, out isfile);
            }
            else if (null != this.Parent)
            {
                return this.Parent.MapPath(path);
            }
            else
            {
                return service.MapPath(this.LoadType, path, string.Empty, out isfile);
            }
        }

        /// <summary>
        /// Overrides the base implementation to set this group as the component owner
        /// </summary>
        /// <returns></returns>
        protected override StyleCollection CreateInnerStyles()
        {
            var col = base.CreateInnerStyles();
            col.Owner = this;

            return col;
        }


        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.InnerItems.Dispose();
            }
        }

    }
}

