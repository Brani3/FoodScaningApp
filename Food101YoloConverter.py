import os
import shutil
import urllib.request
import tarfile

DATASET_DIR = "food-101"
YOLO_DIR = "food101_yolo"
IMAGE_DIR = f"{DATASET_DIR}/images"
TRAIN_LIST = f"{DATASET_DIR}/meta/train.txt"
TEST_LIST = f"{DATASET_DIR}/meta/test.txt"

def download_and_extract():
    if not os.path.exists(DATASET_DIR):
        print("Downloading Food-101...")
        urllib.request.urlretrieve(
            "http://data.vision.ee.ethz.ch/cvl/food-101.tar.gz", "food-101.tar.gz"
        )
        print("Extracting...")
        with tarfile.open("food-101.tar.gz", "r:gz") as tar:
            tar.extractall()
        os.remove("food-101.tar.gz")

def parse_split(file_path):
    with open(file_path, "r") as f:
        return [line.strip() for line in f.readlines()]

def get_class_map():
    return sorted(os.listdir(IMAGE_DIR))

def create_yolo_structure(class_map, split_name, split_list):
    image_out_dir = os.path.join(YOLO_DIR, "images", split_name)
    label_out_dir = os.path.join(YOLO_DIR, "labels", split_name)
    os.makedirs(image_out_dir, exist_ok=True)
    os.makedirs(label_out_dir, exist_ok=True)

    for item in split_list:
        class_name, filename = item.split("/")
        class_id = class_map.index(class_name)
        src_img = os.path.join(IMAGE_DIR, class_name, f"{filename}.jpg")
        dst_img = os.path.join(image_out_dir, f"{class_name}_{filename}.jpg")
        dst_label = os.path.join(label_out_dir, f"{class_name}_{filename}.txt")

        shutil.copyfile(src_img, dst_img)

        with open(dst_label, "w") as f:
            f.write(f"{class_id} 0.5 0.5 1.0 1.0\n")

def create_yaml(class_map):
    with open(os.path.join(YOLO_DIR, "data.yaml"), "w") as f:
        f.write("train: food101_yolo/images/train\n")
        f.write("val: food101_yolo/images/val\n")
        f.write(f"nc: {len(class_map)}\n")
        f.write("names: [")
        f.write(", ".join(f"'{name}'" for name in class_map))
        f.write("]\n")

def main():
    download_and_extract()
    class_map = get_class_map()

    train_list = parse_split(TRAIN_LIST)
    val_list = parse_split(TEST_LIST)

    print("Creating YOLO-compatible structure...")
    create_yolo_structure(class_map, "train", train_list)
    create_yolo_structure(class_map, "val", val_list)
    create_yaml(class_map)
    print("Done! Dataset ready for YOLO training.")

if __name__ == "__main__":
    main()
