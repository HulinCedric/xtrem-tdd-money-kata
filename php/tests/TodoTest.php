<?php

declare(strict_types=1);

namespace Tests;

use MoneyKata\TodoClass;
use PHPUnit\Framework\TestCase;

class TodoTest extends TestCase
{
    public function testFoo(): void
    {
        $todoClass = new TodoClass('foo');
        $this->assertSame('fixme', "{$todoClass}");
    }
}
