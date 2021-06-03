DC=dotnet

all:
	dotnet publish -r linux-x64 -c Release -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true

clean:
	rm -rf bin/
	rm -rf obj/

install:
	install ./bin/Release/net5.0/linux-x64/publish/games-cli /usr/local/bin

uninstall:
	rm -f /usr/local/bin/games-cli
