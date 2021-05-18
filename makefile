DC=dotnet

all:
	dotnet build

clean:
	rm -rf bin/
	rm -rf obj/

install:
	install ./bin/Debug/net5.0/snake-cli /usr/local/bin
	install ./bin/Debug/net5.0/snake-cli.deps.json /usr/local/bin
	install ./bin/Debug/net5.0/snake-cli.dll /usr/local/bin
	install ./bin/Debug/net5.0/snake-cli.pdb /usr/local/bin
	install ./bin/Debug/net5.0/snake-cli.runtimeconfig.dev.json /usr/local/bin
	install ./bin/Debug/net5.0/snake-cli.runtimeconfig.json /usr/local/bin

uninstall:
	rm -f /usr/local/bin/snake-cli
	rm -f /usr/local/bin/snake-cli.deps.json
	rm -f /usr/local/bin/snake-cli.dll
	rm -f /usr/local/bin/snake-cli.pdb
	rm -f /usr/local/bin/snake-cli.runtimeconfig.dev.json
	rm -f /usr/local/bin/snake-cli.runtimeconfig.json
