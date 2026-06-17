"use client";

import { useEffect, useState } from "react";

type Item = {
  id: number;
  text: string;
  is_complited: boolean;
};

export default function Playground() {
  const [items, setItems] = useState<Item[]>([]);
  const [inputValue, setInputValue] = useState<string>("");
  const [countUnfulfilled, setCountUnfulfilled] = useState<number>(0);

  const addItems = () => {
    const newItem: Item = {
      id: items.length + 1,
      text: inputValue,
      is_complited: false,
    };

    setCountUnfulfilled((prev) => prev + 1);

    setItems((items) => [...items, newItem]);
  };

  const markComplete = (id: number) => {
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
          <div key={crypto.randomUUID()}>
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
