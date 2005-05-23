# Makefile for Unix systems.
# Requires mcs, the Mono C# compiler.
# Author: Jo Vermeulen <jo@lumumba.uhasselt.be>

SRC_DIR = Cassowary

all: lib test

lib:
	mcs -warn:4 -target:library -out:Cassowary.dll ${SRC_DIR}/*.cs ${SRC_DIR}/Utils/*.cs
test:
	mcs -warn:4 -target:exe -out:ClTests.exe ${SRC_DIR}/*.cs ${SRC_DIR}/Utils/*.cs

clean:
	rm *.exe 
