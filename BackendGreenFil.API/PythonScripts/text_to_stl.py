import requests
import time
import sys
import os
import trimesh
import numpy as np
from stl import mesh

def main(prompt, output_dir, output_name):
    API_KEY = "msy_yLpQ2Qy3nNBt7kFfgSVPU34qOvrHVdWPAycQ"

    headers = {
        "Authorization": f"Bearer {API_KEY}",
        "Content-Type": "application/json"
    }

    payload = {
        "mode": "preview",
        "prompt": prompt,
        "negative_prompt": "low quality, low resolution",
        "art_style": "realistic",
        "should_remesh": True
    }

    response = requests.post("https://api.meshy.ai/openapi/v2/text-to-3d", headers=headers, json=payload)
    if response.status_code not in [200, 202]:
        raise Exception(f"Error al crear tarea: {response.status_code} - {response.text}")

    task_id = response.json()["result"]

    while True:
        status_response = requests.get(f"https://api.meshy.ai/openapi/v2/text-to-3d/{task_id}", headers=headers)
        task_data = status_response.json()
        status = task_data.get("status")

        if status == "SUCCEEDED":
            glb_url = task_data["model_urls"].get("glb")
            if not glb_url:
                raise Exception("No se encontró la URL del archivo GLB.")

            os.makedirs(output_dir, exist_ok=True)
            glb_path = os.path.join(output_dir, f"{output_name}.glb")

            # Descargar GLB
            glb_response = requests.get(glb_url)
            with open(glb_path, 'wb') as f:
                f.write(glb_response.content)

            # Convertir GLB a STL
            mesh_trimesh = trimesh.load(glb_path)
            temp_stl_path = os.path.join(output_dir, f"{output_name}_temp.stl")
            mesh_trimesh.export(temp_stl_path)

            # Rotar STL como en image_to_stl.py
            your_mesh = mesh.Mesh.from_file(temp_stl_path)
            angle = np.radians(-90)
            rotation_matrix = np.array([
                [1, 0, 0],
                [0, np.cos(angle), -np.sin(angle)],
                [0, np.sin(angle), np.cos(angle)]
            ])
            your_mesh.vectors = np.dot(your_mesh.vectors, rotation_matrix)

            # Guardar STL rotado
            final_stl_path = os.path.join(output_dir, f"{output_name}.stl")
            your_mesh.save(final_stl_path)

            # Limpiar STL temporal
            os.remove(temp_stl_path)

            # Imprimir rutas para backend (.NET)
            print(glb_path)
            print(final_stl_path)
            return

        elif status == "FAILED":
            raise Exception(f"Fallo en la generación: {task_data.get('error', 'Error desconocido')}")

        time.sleep(5)

if __name__ == "__main__":
    if len(sys.argv) < 4:
        print("Uso: python text_to_stl.py <prompt> <output_dir> <nombre_archivo>", file=sys.stderr)
        sys.exit(1)

    try:
        prompt = sys.argv[1]
        output_dir = sys.argv[2]
        output_name = sys.argv[3]
        main(prompt, output_dir, output_name)
    except Exception as e:
        print(f"Error: {str(e)}", file=sys.stderr)
        sys.exit(1)
