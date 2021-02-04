export function CustomNamed(customName: string){
  return (constructor: any) => {
    Object.defineProperty(constructor, '__customName', {
      enumerable: false,
      configurable: false,
      writable: false,
      value: customName
    });
  };
}
