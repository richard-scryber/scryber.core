from recommonmark.parser import CommonMarkParser
source_parsers = {
'.md': CommonMarkParser,
}
source_suffix = ['.rst', '.md']
master_doc = 'index' 
project = u'Scryber Core'
html_static_path = ['_static']

html_css_files = [
    'css/scrybertheme.css',
]