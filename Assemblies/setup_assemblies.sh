#!/usr/bin/env bash

for f in `ls *.tmpl`; do
  new_f=${f//_//}
  new_f=${new_f/.tmpl/}
  echo mv $f ../$new_f
done
