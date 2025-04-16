# WieGehtCsConsole
Kursmaterial zum LinkedIn-Kurs: Wie geht - C# Konsolenanwendungen

Zur Installation von .Net und VSCode auf den anderen Plattformen:

Linux:
1. Einige Abhängigkeiten und Tools:
sudo apt install -y wget apt-transport-https software-properties-common
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
2. .Net SDK
Sudo apt install –y dotnet-sdk-8.0
3. Testen
dotnet –-version
4. Vscode installieren
wget -q https://packages.microsoft.com/keys/microsoft.asc -O- | sudo apt-key add -
sudo add-apt-repository "deb [arch=amd64] https://packages.microsoft.com/repos/vscode stable main" 
sudo apt update
sudo apt install code
5. C#-Erweiterungen installieren


MacOS:
````
brew update 
brew install --cask dotnet-sdk
dotnet --version 
export PATH="$PATH:/usr/local/share/dotnet/dotnet/64“
````
VSCode von https://code.visualstudio.com/docs/setup/mac

