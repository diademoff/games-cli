# About
![GitHub repo size](https://img.shields.io/github/repo-size/diademoff/snake-cli)
![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/diademoff/snake-cli)
![GitHub](https://img.shields.io/github/license/diademoff/snake-cli)

Play your favorite game in console written in .NET Core.

<img src="https://i.imgur.com/Yovm1S1.png" alt="drawing" width="600"/>

Platforms:
* Windows
* Gnu Linux

[Download](https://github.com/diademoff/snake-cli/releases)

# Build
Install `dotnet-runtime` and `dotnet-sdk` to build.

## Linux

```
git clone https://github.com/diademoff/snake-cli && cd snake-cli
```

Build and install
```
make && sudo make install
```

Run
```
snake-cli
```

## Build for Windows
```
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
```
