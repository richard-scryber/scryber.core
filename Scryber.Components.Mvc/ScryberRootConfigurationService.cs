using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scryber.Options;

namespace Scryber.Components.Mvc
{
    internal class ScryberRootConfigurationService : IScryberConfigurationService
    {

        private IConfiguration _config;
        private object _lock = new object();
        private ParsingOptions _parser;
        private FontOptions _font;
        private ImagingOptions _imaging;
        private OutputOptions _output;
        private TracingOptions _tracelog;

        public ParsingOptions ParsingOptions
        {
            get
            {
                lock (_lock)
                {
                    if (null == _parser)
                    {
                        _parser = this.GetOptions<ParsingOptions>(Options.ParsingOptions.ParsingSection);
                        if (null == _parser)
                            _parser = new Options.ParsingOptions();
                    }
                }

                return _parser;
            }
        }

        public FontOptions FontOptions
        {
            get
            {
                lock (_lock)
                {
                    if (null == _font)
                    {
                        _font = this.GetOptions<FontOptions>(Options.FontOptions.FontsSection);
                        if (null == _font)
                            _font = new FontOptions();
                    }
                }

                return _font;
            }
        }



        public ImagingOptions ImagingOptions
        {
            get
            {
                lock (_lock)
                {
                    if (null == _imaging)
                    {
                        _imaging = this.GetOptions<ImagingOptions>(Options.ImagingOptions.ImagingSection);
                        if (null == _imaging)
                            _imaging = new ImagingOptions();
                    }
                }

                return _imaging;
            }
        }

        public OutputOptions OutputOptions
        {
            get
            {
                lock (_lock)
                {
                    if (null == _output)
                    {
                        _output = this.GetOptions<OutputOptions>(Options.OutputOptions.OutputSection);
                        if (null == _output)
                            _output = new OutputOptions();
                    }
                }

                return _output;
            }
        }

        public TracingOptions TracingOptions
        {
            get
            {
                lock (_lock)
                {
                    if (null == _tracelog)
                    {
                        _tracelog = this.GetOptions<TracingOptions>(Options.TracingOptions.TracingSection);
                        if (null == _tracelog)
                            _tracelog = new TracingOptions();
                    }
                }

                return _tracelog;
            }
        }

        //
        // .ctor
        //

        public ScryberRootConfigurationService(IConfiguration configuration)
        {
            _config = configuration;
        }

        //
        // methods
        //

        private T GetOptions<T>(string path) where T : new()
        {
            var section = _config.GetSection(path);
            var value = new T();
            return value;
        }


        internal bool TryGetConfiguration(out IConfiguration config)
        {
            config = ServiceProvider.GetService<IConfiguration>();
            return null != config;
        }


        object IScryberConfigurationService.GetScryberSection(Type ofType, string name)
        {
            lock (_lock)
            {
                IConfiguration config;
                if (this.TryGetConfiguration(out config))
                {
                    object value = config.GetSection(ScryberOptions.ScryberSectionStub + name);
                    return value;
                }
                else
                    return null;
            }
        }

        public void Reset()
        {
            lock (_lock)
            {
                _parser = null;
                _font = null;
                _imaging = null;
                _output = null;
                _tracelog = null;
            }
        }

    }
}
