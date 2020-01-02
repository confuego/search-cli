# Search CLI

## Getting Started

```code
git clone https://github.com/confuego/search-cli.git
cd search-cli
docker build . -t search
docker run -it search /bin/bash
search -i ./data/sample_data.csv.gz
```

## Arguments

```code
--index -i /path/to/file => makes a file searchable
```

## Commands

###**Make sure column names DO NOT include (=, ^, @, >)**

```code
> {text} | runs a general search for those keywords & sorts by relevance
> {column} = {text} | search for exact match on text in a particular column & sort by relevance
> {column} @> {text} | search for column to contain text & sort by relevance
> {column} ^= {text} | search for column that starts with text & sort by relevance
```

