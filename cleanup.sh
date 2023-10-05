#!/usr/bin/env bash

# Put this script on the parent folder and run in a linux shell (e.g.: git bash or WSL) ./cleanup.sh
echo "This command will delete all 'obj' and 'bin' folders from the current directory you're curently on.";
read -p "Do you want to proceed? (Y\n): " userChoice;

if [ "$userChoice" != "Y" ]
then
        echo "The command was aborted!";
        exit 1;
fi

binFolders=$(find . -maxdepth 50 -type d -name 'bin' -prune -not -path "*/node_modules/*");
objFolders=$(find . -maxdepth 50 -type d -name 'obj' -prune -not -path "*/node_modules/*");

for folder in $binFolders
do
        rm -rf $folder
        echo $folder
done

for folder in $objFolders
do
        rm -rf $folder
        echo $folder
done

echo "";
echo "The command has finished!"
exit 0;