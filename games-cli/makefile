DC=dotnet

all:
	dotnet build

clean:
	rm -rf bin/
	rm -rf obj/

install:
	install ./bin/Debug/net5.0/games-cli /usr/local/bin
	install ./bin/Debug/net5.0/games-cli.deps.json /usr/local/bin
	install ./bin/Debug/net5.0/games-cli.dll /usr/local/bin
	install ./bin/Debug/net5.0/games-cli.pdb /usr/local/bin
	install ./bin/Debug/net5.0/games-cli.runtimeconfig.dev.json /usr/local/bin
	install ./bin/Debug/net5.0/games-cli.runtimeconfig.json /usr/local/bin

uninstall:
	rm -f /usr/local/bin/games-cli
	rm -f /usr/local/bin/games-cli.deps.json
	rm -f /usr/local/bin/games-cli.dll
	rm -f /usr/local/bin/games-cli.pdb
	rm -f /usr/local/bin/games-cli.runtimeconfig.dev.json
	rm -f /usr/local/bin/games-cli.runtimeconfig.json
