import sys
import json
import os

# Protege os imports mais pesados
try:
    import easyocr
    import cv2
except ImportError as e:
    print(f" ERRO: Biblioteca não instalada - {e.name}", flush=True)
    exit(1)

if len(sys.argv) < 2:
    print(" Caminho da imagem não informado.", flush=True)
    exit(1)

image_path = sys.argv[1]
print("Lendo:", image_path, flush=True)

if not os.path.exists(image_path):
    print(" Arquivo não encontrado!", flush=True)
    exit(1)

# Tenta abrir a imagem
img = cv2.imread(image_path)
if img is None:
    print(" ERRO: OpenCV não conseguiu ler a imagem.", flush=True)
    exit(1)

try:
    reader = easyocr.Reader(['pt'], verbose=False)
    results = reader.readtext(image_path)
    texts = [text for (_, text, _) in results]
    print(json.dumps(texts, ensure_ascii=False), flush=True)
except Exception as e:
    print(f" ERRO durante OCR: {e}", flush=True)
    exit(1)
