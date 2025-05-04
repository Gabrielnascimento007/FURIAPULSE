import sys
import requests
from bs4 import BeautifulSoup

if len(sys.argv) < 2:
    print("NAO_INFORMADO", flush=True)
    exit()

url = sys.argv[1]

try:
    headers = {
        "User-Agent": "Mozilla/5.0"
    }
    r = requests.get(url, headers=headers, timeout=10)
    soup = BeautifulSoup(r.text, "html.parser")
    meta = soup.find("meta", attrs={"property": "og:description"})
    bio = meta["content"] if meta and "content" in meta.attrs else ""
except Exception as e:
    print("ERRO", flush=True)
    exit()

# Termos relacionados à FURIA
mencoes_furia = [
    "@furia", "@furiaesports", "@furia.football", "@furia.redram", "@furia.lol",
    "@furia.valorant", "@furia.r6", "@furia.apparel", "@themovegg"
]

bio_lower = bio.lower()
if any(tag in bio_lower for tag in mencoes_furia):
    print("APOIA", flush=True)
else:
    print("NAO_APOIA", flush=True)
