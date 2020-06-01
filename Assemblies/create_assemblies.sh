#!/usr/bin/env bash

for f in `find ../Assets -name '*.asmdef'`; do
  new_f=${f/\.\.\//}
  new_f=${new_f//\//_}
  mv $f ./$new_f.tmpl
done
