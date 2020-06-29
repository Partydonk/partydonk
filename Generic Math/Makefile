.PHONY: all
all: Numerics.svg

Numerics.dot: Numerics.dot.m4
	@echo '// Auto-generated from $@ on $(shell date)' > $@
	@echo '// Do Not Edit - see Makefile' >> $@
	m4 < $< >> $@

Numerics.svg: Numerics.dot
	dot $< -Tsvg > $@

.PHONY: clean
clean:
	rm -f Numerics.dot Numerics.svg
