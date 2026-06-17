"use client";

import { useEffect, useState } from "react";

type Item = {
  id: string;
  text: string;
  is_complited: boolean;
};

export default function Playground() {
  const [items, setItems] = useState<Item[]>([]);
  const [inputValue, setInputValue] = useState<string>("");
  const countUnfulfilled = items.filter((item) => !item.is_complited).length;

  const addItems = () => {
    if (!inputValue.trim()) return;

    const newItem: Item = {
      id: crypto.randomUUID(),
      text: inputValue,
      is_complited: false,
    };

    setItems((items) => [...items, newItem]);

    setInputValue("");
  };

  const markComplete = (id: string) => {
    setItems((prev) =>
      prev.map((item) => {
        if (item.id == id) {
          return { ...item, is_complited: !item.is_complited };
        }
        return item;
      }),
    );
  };

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Playground</h1>
      <div>Количество невыполненных задач {countUnfulfilled}</div>
      <input
        type="text"
        onChange={(event) => setInputValue(event.target.value)}
      />
      <button onClick={addItems}>Добавить</button>

      <div>
        {items.map((item) => (
          <div key={item.id}>
            <h1>{item.text}</h1>
            <div>{item.is_complited ? "ok" : "ne ok"}</div>
            <input
              type="checkbox"
              checked={item.is_complited}
              onChange={() => markComplete(item.id)}
            />
          </div>
        ))}
      </div>
    </div>
  );
}
