<?php

declare(strict_types=1);

namespace MoneyKata;

final class TodoClass
{
    private string $name;

    public function __construct(string $name,)
    {
        $this->name = $name;
    }

    public function __toString(): string
    {
        return "{$this->name}";
    }
}
