Play your favorite game in console written in .NET Core.

<img src="https://i.imgur.com/Yovm1S1.png" alt="drawing" width="600"/>

Platforms:
* Windows
* Gnu Linux

[Download](https://github.com/diademoff/snake-cli/releases)

**Build**

Dependencies (Arch linux):
```
pacman -S dotnet-runtime dotnet-sdk
```

```
git clone https://github.com/diademoff/snake-cli
```

```
cd snake-cli
```

```
make && sudo make install
```

Run:
```
snake-cli
```

**Build for Windows**
```
dotnet build --runtime win-x64
```