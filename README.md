# About
![GitHub repo size](https://img.shields.io/github/repo-size/diademoff/games-cli)
![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/diademoff/games-cli)
![GitHub](https://img.shields.io/github/license/diademoff/games-cli)

Play your favorite game in console. Install `dotnet-runtime` to run app.

<img src="https://i.imgur.com/Yovm1S1.png" alt="drawing" width="600"/>

Platforms:
* Windows
* Gnu Linux

Features:
* Use `W`, `A`, `S`, `D` or arrows for moving
* Use `Escape` for pause
* Use `Space` for speedup

[Download](https://github.com/diademoff/games-cli/releases)

# Build
Install `dotnet-sdk` to build.

## Linux

```
git clone https://github.com/diademoff/games-cli && cd games-cli/games-cli
```

Build and install
```
make && sudo make install
```

Run
```
snake-cli
```

## Windows
```
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
```
