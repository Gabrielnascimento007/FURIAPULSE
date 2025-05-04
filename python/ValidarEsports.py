import sys
import requests
from bs4 import BeautifulSoup

if len(sys.argv) < 2:
    print("NAO_INFORMADO", flush=True)
    exit()

url = sys.argv[1]

try:
    headers = { "User-Agent": "Mozilla/5.0" }
    r = requests.get(url, headers=headers, timeout=10)
    html = r.text.lower()

    termos_esports = ["cs2", "csgo", "valorant", "lol", "league of legends", "faceit", "esports", "furia", "champion", "match", "ranking", "elo"]

    if any(term in html for term in termos_esports):
        print("ESPORTS_OK", flush=True)
    else:
        print("SEM_CONTEUDO_ESPORTS", flush=True)
except Exception as e:
    print("ERRO", flush=True)
