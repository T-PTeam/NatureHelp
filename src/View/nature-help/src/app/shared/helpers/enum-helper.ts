export function enumToSelectOptions<T extends object>(enumObj: T): { value: T[keyof T]; label: string }[] {
  const result = Object.keys(enumObj)
    .filter((key) => isNaN(Number(key)))
    .map((key) => {
      const value = enumObj[key as keyof typeof enumObj];
      return { value, label: key };
    });

  return result;
}
