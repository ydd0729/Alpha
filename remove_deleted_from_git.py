import os
import subprocess
import re

def run_powershell_command_live(command):
    # 启动 PowerShell 进程
    process = subprocess.Popen(
        ['pwsh', '-NoProfile', '-Command', command],
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        text=True
    )
    
    # 逐行读取输出并打印，以实现实时显示
    for line in iter(process.stdout.readline, ''):
        print(line, end='')
    
    # 关闭输出流并等待进程结束
    process.stdout.close()
    return_code = process.wait()

    # 检查返回码，如果有错误则抛出异常
    if return_code:
        raise subprocess.CalledProcessError(return_code, command)

run_powershell_command_live("git-filter-repo --analyze --force")

script_path = os.path.abspath(__file__)
script_dir = os.path.dirname(script_path)
base_path = os.path.join(script_dir, '.git', 'filter-repo', 'analysis')

deleted_paths = []
with open(os.path.join(base_path, "path-deleted-sizes.txt"), "r") as f:
    for line in f.readlines():
        match = re.search(r'\s+\d+\s+\d+\s+\d+-\d+-\d+\s(.*)', line)
        match = match.group(1) if match else ''
        if not match.endswith('.cs'):
            deleted_paths.append(match)

deleted_hashes = []
with open(os.path.join(script_dir, "deleted_hashes.txt"), "w") as w:
    with open(os.path.join(base_path, "blob-shas-and-paths.txt"), "r") as r:
        for line in r.readlines():
            match = re.search(r'\s*(\S+)\s+\d+\s+\d+\s(?:\[)?([^\]|\n]+)', line)
            if match:
                hash = match.group(1)
                paths = match.group(2).split(",")
                for i in range(len(paths)):
                    paths[i] = paths[i].strip()
                
                deleted = True
                for path in paths:
                    if path not in deleted_paths:
                        deleted = False

                if deleted:
                    print(hash, end=' ')
                    print(paths)
                    deleted_hashes.append(hash + '\n')
    
    # print(deleted_hashes)
    w.writelines(deleted_hashes)

os.remove(os.path.join(script_dir, '.git', 'filter-repo', 'already_ran'))
run_powershell_command_live("git-filter-repo --strip-blobs-with-ids \"{}\" --force".format(os.path.join(script_dir, "deleted_hashes.txt")))
os.remove(os.path.join(script_dir, "deleted_hashes.txt"))