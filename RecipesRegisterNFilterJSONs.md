# Exemplos de Requests de Receitas e Filtros

Este documento contém exemplos organizados para **testes de API**, incluindo:

* Registros de receitas (`RequestRecipeJson`)
* Filtros de busca (`RequestFilterRecipeJson`)

Considerando que existem **3 receitas cadastradas** no sistema.

---

# 1. Exemplos de Registro de Receitas (RequestRecipeJson)

## 1.1 Panqueca Americana

```json
{
  "title": "Panqueca Americana",
  "cookingTime": 1,
  "difficulty": 0,
  "ingredients": [
    "1 xícara de farinha de trigo",
    "1 colher de sopa de açúcar",
    "1 colher de chá de fermento em pó",
    "1 pitada de sal",
    "1 ovo",
    "1 xícara de leite",
    "1 colher de sopa de manteiga derretida"
  ],
  "instructions": [
    {
      "step": 1,
      "text": "Misture a farinha, açúcar, fermento e sal em uma tigela."
    },
    {
      "step": 2,
      "text": "Adicione o ovo, leite e manteiga derretida e misture até formar uma massa homogênea."
    },
    {
      "step": 3,
      "text": "Aqueça uma frigideira untada e despeje pequenas porções da massa."
    },
    {
      "step": 4,
      "text": "Cozinhe até formar bolhas e vire para dourar o outro lado."
    }
  ],
  "dishTypes": [0, 3]
}
```

---

## 1.2 Spaghetti à Carbonara

```json
{
  "title": "Spaghetti à Carbonara",
  "cookingTime": 2,
  "difficulty": 1,
  "ingredients": [
    "200g de spaghetti",
    "100g de bacon ou pancetta",
    "2 ovos",
    "50g de queijo parmesão ralado",
    "Pimenta-do-reino a gosto",
    "Sal a gosto"
  ],
  "instructions": [
    {
      "step": 1,
      "text": "Cozinhe o spaghetti em água salgada conforme instruções da embalagem."
    },
    {
      "step": 2,
      "text": "Frite o bacon em uma frigideira até ficar dourado."
    },
    {
      "step": 3,
      "text": "Misture os ovos com o parmesão e pimenta em uma tigela."
    },
    {
      "step": 4,
      "text": "Escorra o macarrão e misture rapidamente com o bacon e a mistura de ovos fora do fogo."
    }
  ],
  "dishTypes": [1]
}
```

---

## 1.3 Bolo de Cenoura com Cobertura de Chocolate

```json
{
  "title": "Bolo de Cenoura com Cobertura de Chocolate",
  "cookingTime": 3,
  "difficulty": 1,
  "ingredients": [
    "3 cenouras médias",
    "3 ovos",
    "1 xícara de óleo",
    "2 xícaras de açúcar",
    "2 xícaras de farinha de trigo",
    "1 colher de sopa de fermento em pó",
    "1 lata de leite condensado",
    "3 colheres de sopa de chocolate em pó",
    "1 colher de sopa de manteiga"
  ],
  "instructions": [
    {
      "step": 1,
      "text": "Bata no liquidificador as cenouras, ovos e óleo."
    },
    {
      "step": 2,
      "text": "Misture com o açúcar e a farinha em uma tigela."
    },
    {
      "step": 3,
      "text": "Adicione o fermento e mexa delicadamente."
    },
    {
      "step": 4,
      "text": "Asse em forno pré-aquecido a 180°C por cerca de 40 minutos."
    },
    {
      "step": 5,
      "text": "Para a cobertura, leve ao fogo o leite condensado, chocolate e manteiga até engrossar."
    }
  ],
  "dishTypes": [4, 5]
}
```

---

# 2. Exemplos de Filtros de Busca (RequestFilterRecipeJson)

## 2.1 Filtro vazio (retorna todas as receitas)

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": null,
  "difficulties": null,
  "dishTypes": null
}
```

---

## 2.2 Buscar por título

```json
{
  "recipeTitle_Ingredient": "Panqueca",
  "cookingTimes": null,
  "difficulties": null,
  "dishTypes": null
}
```

---

## 2.3 Buscar por ingrediente

```json
{
  "recipeTitle_Ingredient": "cenoura",
  "cookingTimes": null,
  "difficulties": null,
  "dishTypes": null
}
```

---

## 2.4 Filtrar por tempo de preparo

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": [1],
  "difficulties": null,
  "dishTypes": null
}
```

---

## 2.5 Filtrar por múltiplos tempos

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": [1, 2],
  "difficulties": null,
  "dishTypes": null
}
```

---

## 2.6 Filtrar por dificuldade fácil

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": null,
  "difficulties": [0],
  "dishTypes": null
}
```

---

## 2.7 Filtrar por dificuldade média

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": null,
  "difficulties": [1],
  "dishTypes": null
}
```

---

## 2.8 Filtrar por tipo de prato

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": null,
  "difficulties": null,
  "dishTypes": [1]
}
```

---

## 2.9 Filtrar por múltiplos tipos de prato

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": null,
  "difficulties": null,
  "dishTypes": [4, 5]
}
```

---

## 2.10 Filtro combinado (tempo + dificuldade)

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": [2],
  "difficulties": [1],
  "dishTypes": null
}
```

---

## 2.11 Filtro combinado com texto

```json
{
  "recipeTitle_Ingredient": "chocolate",
  "cookingTimes": [3],
  "difficulties": [1],
  "dishTypes": [4]
}
```

---

## 2.12 Filtro que não retorna resultados

```json
{
  "recipeTitle_Ingredient": "frango",
  "cookingTimes": null,
  "difficulties": null,
  "dishTypes": null
}
```

---

## 2.13 Combinação impossível

```json
{
  "recipeTitle_Ingredient": null,
  "cookingTimes": [0],
  "difficulties": [2],
  "dishTypes": [6]
}
```

---
