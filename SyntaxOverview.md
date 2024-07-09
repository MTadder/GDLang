* Every statement in MTLang will occupy a `Chunk`.
* Each `Chunk` will correspond to a line of code in `Gdscript`.
* All newlines and whitespace are ignored in MTLang.
* Chronology is determined from the literal call sequence.
* A `Chunk` is denoted by the `{` and `}` characters.
# Syntax Overview
## Variable Declarations (with getters/setters)
* `^variable_name{type[, instantiation_params]}^`
* `$variable_name{('set' | 'get')[, Chunk]}$`
## Class Declarations (with parameters)
* `&class_name[{[parameters]}][::extend_class{[parameters]}]&`
## Object / Scene instantiation (with parameters)
* `<object_name{[parameters]}>` -- calls `object_name.new([parameters]);`.
* `@<scene_name{[parameters]}>` -- sets the scene tree to this packed scene file (.tscn | .scn).
## Flow Control
* `branch clause_chunk : (if_true, if_false)`
## Examples
* `{inplace{"other_file.mtl"}};` -- Copies the MTLang code from `other_file.mtl` to this corresponding `Chunk`.
* `{&Tetrad{x, y, z, o}::Dyad{!}&}` -- Declares class `Tetrad` which extends the `Dyad` class.